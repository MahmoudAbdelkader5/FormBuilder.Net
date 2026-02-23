-- =============================================
-- Stored Procedure: sp_EvaluateBlockingRules
-- Purpose: Generic stored procedure to evaluate blocking rules for forms/documents
-- =============================================
CREATE OR ALTER PROCEDURE [dbo].[sp_EvaluateBlockingRules]
    @FormId INT,
    @EvaluationPhase NVARCHAR(20), -- 'PreOpen' or 'PreSubmit'
    @SubmissionId INT = NULL,
    @IsBlocked BIT OUTPUT,
    @BlockMessage NVARCHAR(1000) OUTPUT,
    @MatchedRuleId INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Initialize output parameters
    SET @IsBlocked = 0;
    SET @BlockMessage = NULL;
    SET @MatchedRuleId = NULL;
    
    -- Get active blocking rules for the form and phase
    DECLARE @BlockingRules TABLE (
        RuleId INT,
        RuleName NVARCHAR(200),
        ConditionSource NVARCHAR(20),
        ConditionKey NVARCHAR(100),
        ConditionOperator NVARCHAR(50),
        ConditionValue NVARCHAR(500),
        BlockMessage NVARCHAR(1000),
        Priority INT,
        RuleType NVARCHAR(50),
        StoredProcedureId INT,
        StoredProcedureName NVARCHAR(200),
        StoredProcedureDatabase NVARCHAR(100)
    );
    
    INSERT INTO @BlockingRules
    SELECT 
        r.Id,
        r.RuleName,
        r.ConditionSource,
        r.ConditionKey,
        r.ConditionOperator,
        r.ConditionValue,
        r.BlockMessage,
        r.Priority,
        r.RuleType,
        r.StoredProcedureId,
        r.StoredProcedureName,
        r.StoredProcedureDatabase
    FROM FORM_RULES r
    WHERE r.FormBuilderId = @FormId
        AND r.IsActive = 1
        AND r.IsDeleted = 0
        AND r.EvaluationPhase = @EvaluationPhase
        AND r.ConditionSource IS NOT NULL
    ORDER BY r.Priority DESC, r.ExecutionOrder ASC;
    
    -- Evaluate each rule
    DECLARE @CurrentRuleId INT;
    DECLARE @CurrentRuleName NVARCHAR(200);
    DECLARE @CurrentConditionSource NVARCHAR(20);
    DECLARE @CurrentConditionKey NVARCHAR(100);
    DECLARE @CurrentConditionOperator NVARCHAR(50);
    DECLARE @CurrentConditionValue NVARCHAR(500);
    DECLARE @CurrentBlockMessage NVARCHAR(1000);
    DECLARE @CurrentRuleType NVARCHAR(50);
    DECLARE @CurrentStoredProcedureId INT;
    DECLARE @CurrentStoredProcedureName NVARCHAR(200);
    DECLARE @CurrentStoredProcedureDatabase NVARCHAR(100);
    DECLARE @ConditionMet BIT = 0;
    
    DECLARE rule_cursor CURSOR FOR
    SELECT RuleId, RuleName, ConditionSource, ConditionKey, ConditionOperator, 
           ConditionValue, BlockMessage, RuleType, StoredProcedureId, 
           StoredProcedureName, StoredProcedureDatabase
    FROM @BlockingRules;
    
    OPEN rule_cursor;
    FETCH NEXT FROM rule_cursor INTO 
        @CurrentRuleId, @CurrentRuleName, @CurrentConditionSource, 
        @CurrentConditionKey, @CurrentConditionOperator, @CurrentConditionValue,
        @CurrentBlockMessage, @CurrentRuleType, @CurrentStoredProcedureId,
        @CurrentStoredProcedureName, @CurrentStoredProcedureDatabase;
    
    WHILE @@FETCH_STATUS = 0 AND @IsBlocked = 0
    BEGIN
        SET @ConditionMet = 0;
        
        -- Evaluate based on ConditionSource
        IF @CurrentConditionSource = 'Database'
        BEGIN
            -- Database-based rules: Evaluate using stored procedure or direct DB query
            IF @CurrentRuleType = 'StoredProcedure' AND @CurrentStoredProcedureId IS NOT NULL
            BEGIN
                -- Call the stored procedure specified in the rule
                -- Note: This is a simplified example. In practice, you would need to
                -- dynamically execute the stored procedure based on the rule configuration
                -- For now, we'll set a flag that the condition needs to be evaluated
                -- by the application layer
                SET @ConditionMet = 0; -- Placeholder - actual evaluation done in application
            END
            ELSE IF @CurrentRuleType = 'StoredProcedure' 
                    AND @CurrentStoredProcedureName IS NOT NULL 
                    AND @CurrentStoredProcedureDatabase IS NOT NULL
            BEGIN
                -- Similar to above - placeholder for dynamic SP execution
                SET @ConditionMet = 0;
            END
            ELSE
            BEGIN
                -- Direct database condition evaluation using ConditionKey
                -- Example: Check if accounting period is closed
                -- This would need to be customized based on your business logic
                SET @ConditionMet = 0; -- Placeholder
            END
        END
        ELSE IF @CurrentConditionSource = 'Submission' AND @SubmissionId IS NOT NULL
        BEGIN
            -- Submission-based rules: Evaluate using submission field values
            DECLARE @FieldValue NVARCHAR(MAX);
            DECLARE @CompareValue NVARCHAR(500) = @CurrentConditionValue;
            
            -- Get field value from submission
            SELECT TOP 1 @FieldValue = fs.Value
            FROM FORM_SUBMISSION_VALUES fs
            WHERE fs.SubmissionId = @SubmissionId
                AND fs.FieldCode = @CurrentConditionKey
                AND fs.IsDeleted = 0
            ORDER BY fs.Id DESC;
            
            -- Evaluate condition based on operator
            IF @CurrentConditionOperator = '='
            BEGIN
                SET @ConditionMet = CASE WHEN @FieldValue = @CompareValue THEN 1 ELSE 0 END;
            END
            ELSE IF @CurrentConditionOperator = '!='
            BEGIN
                SET @ConditionMet = CASE WHEN @FieldValue != @CompareValue THEN 1 ELSE 0 END;
            END
            ELSE IF @CurrentConditionOperator = '>'
            BEGIN
                SET @ConditionMet = CASE WHEN CAST(@FieldValue AS DECIMAL(18,2)) > CAST(@CompareValue AS DECIMAL(18,2)) THEN 1 ELSE 0 END;
            END
            ELSE IF @CurrentConditionOperator = '<'
            BEGIN
                SET @ConditionMet = CASE WHEN CAST(@FieldValue AS DECIMAL(18,2)) < CAST(@CompareValue AS DECIMAL(18,2)) THEN 1 ELSE 0 END;
            END
            ELSE IF @CurrentConditionOperator = '>='
            BEGIN
                SET @ConditionMet = CASE WHEN CAST(@FieldValue AS DECIMAL(18,2)) >= CAST(@CompareValue AS DECIMAL(18,2)) THEN 1 ELSE 0 END;
            END
            ELSE IF @CurrentConditionOperator = '<='
            BEGIN
                SET @ConditionMet = CASE WHEN CAST(@FieldValue AS DECIMAL(18,2)) <= CAST(@CompareValue AS DECIMAL(18,2)) THEN 1 ELSE 0 END;
            END
        END
        
        -- If condition is met, block and return
        IF @ConditionMet = 1
        BEGIN
            SET @IsBlocked = 1;
            SET @BlockMessage = @CurrentBlockMessage;
            SET @MatchedRuleId = @CurrentRuleId;
            BREAK; -- Exit loop on first match (highest priority)
        END
        
        FETCH NEXT FROM rule_cursor INTO 
            @CurrentRuleId, @CurrentRuleName, @CurrentConditionSource, 
            @CurrentConditionKey, @CurrentConditionOperator, @CurrentConditionValue,
            @CurrentBlockMessage, @CurrentRuleType, @CurrentStoredProcedureId,
            @CurrentStoredProcedureName, @CurrentStoredProcedureDatabase;
    END
    
    CLOSE rule_cursor;
    DEALLOCATE rule_cursor;
END
GO

