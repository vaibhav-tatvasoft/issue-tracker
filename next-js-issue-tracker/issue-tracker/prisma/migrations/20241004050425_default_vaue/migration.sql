BEGIN TRY

BEGIN TRAN;

-- AlterTable
ALTER TABLE [dbo].[Issue] ADD CONSTRAINT [Issue_status_df] DEFAULT 'OPEN' FOR [status];

COMMIT TRAN;

END TRY
BEGIN CATCH

IF @@TRANCOUNT > 0
BEGIN
    ROLLBACK TRAN;
END;
THROW

END CATCH