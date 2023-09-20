CREATE TABLE IF NOT EXISTS `__EFMigrationsHistory` (
    `MigrationId` varchar(150) CHARACTER SET utf8mb4 NOT NULL,
    `ProductVersion` varchar(32) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK___EFMigrationsHistory` PRIMARY KEY (`MigrationId`)
) CHARACTER SET=utf8mb4;

START TRANSACTION;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230919203335_initMig') THEN

    ALTER DATABASE CHARACTER SET utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230919203335_initMig') THEN

    CREATE TABLE `Organizations` (
        `OrganizationId` bigint NOT NULL AUTO_INCREMENT,
        `name` longtext CHARACTER SET utf8mb4 NOT NULL,
        `lunch_price` double NOT NULL,
        `currency` longtext CHARACTER SET utf8mb4 NOT NULL,
        CONSTRAINT `PK_Organizations` PRIMARY KEY (`OrganizationId`)
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230919203335_initMig') THEN

    CREATE TABLE `Invites` (
        `Id` bigint NOT NULL AUTO_INCREMENT,
        `email` longtext CHARACTER SET utf8mb4 NOT NULL,
        `token` longtext CHARACTER SET utf8mb4 NOT NULL,
        `OrganizationId` bigint NOT NULL,
        CONSTRAINT `PK_Invites` PRIMARY KEY (`Id`),
        CONSTRAINT `FK_Invites_Organizations_OrganizationId` FOREIGN KEY (`OrganizationId`) REFERENCES `Organizations` (`OrganizationId`) ON DELETE CASCADE
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230919203335_initMig') THEN

    CREATE TABLE `OrganizationWallets` (
        `WalletId` bigint NOT NULL AUTO_INCREMENT,
        `balance` double NOT NULL,
        `OrganizationId` bigint NOT NULL,
        CONSTRAINT `PK_OrganizationWallets` PRIMARY KEY (`WalletId`),
        CONSTRAINT `FK_OrganizationWallets_Organizations_OrganizationId` FOREIGN KEY (`OrganizationId`) REFERENCES `Organizations` (`OrganizationId`) ON DELETE CASCADE
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230919203335_initMig') THEN

    CREATE TABLE `Users` (
        `UserId` bigint NOT NULL AUTO_INCREMENT,
        `first_name` longtext CHARACTER SET utf8mb4 NOT NULL,
        `last_name` longtext CHARACTER SET utf8mb4 NOT NULL,
        `OrganizationId` bigint NOT NULL,
        `profile_picture` longtext CHARACTER SET utf8mb4 NOT NULL,
        `email` longtext CHARACTER SET utf8mb4 NOT NULL,
        `password_hash` longtext CHARACTER SET utf8mb4 NOT NULL,
        `password_salt` longtext CHARACTER SET utf8mb4 NOT NULL,
        `refresh_token` longtext CHARACTER SET utf8mb4 NOT NULL,
        `bank_number` longtext CHARACTER SET utf8mb4 NOT NULL,
        `bank_code` longtext CHARACTER SET utf8mb4 NOT NULL,
        `bank_name` longtext CHARACTER SET utf8mb4 NOT NULL,
        `Description` longtext CHARACTER SET utf8mb4 NOT NULL,
        `Created_at` datetime(6) NOT NULL,
        `updated_at` datetime(6) NOT NULL,
        `is_admin` tinyint(1) NOT NULL,
        CONSTRAINT `PK_Users` PRIMARY KEY (`UserId`),
        CONSTRAINT `FK_Users_Organizations_OrganizationId` FOREIGN KEY (`OrganizationId`) REFERENCES `Organizations` (`OrganizationId`) ON DELETE CASCADE
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230919203335_initMig') THEN

    CREATE TABLE `Lunches` (
        `LunchId` bigint NOT NULL AUTO_INCREMENT,
        `senderId` bigint NOT NULL,
        `recieverId` bigint NOT NULL,
        `quantity` int NOT NULL,
        `created_at` datetime(6) NOT NULL,
        `note` longtext CHARACTER SET utf8mb4 NOT NULL,
        `UserId` bigint NULL,
        CONSTRAINT `PK_Lunches` PRIMARY KEY (`LunchId`),
        CONSTRAINT `FK_Lunches_Organizations_recieverId` FOREIGN KEY (`recieverId`) REFERENCES `Organizations` (`OrganizationId`) ON DELETE CASCADE,
        CONSTRAINT `FK_Lunches_Organizations_senderId` FOREIGN KEY (`senderId`) REFERENCES `Organizations` (`OrganizationId`) ON DELETE CASCADE,
        CONSTRAINT `FK_Lunches_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `Users` (`UserId`)
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230919203335_initMig') THEN

    CREATE TABLE `Withdrawals` (
        `WithdrawalId` bigint NOT NULL AUTO_INCREMENT,
        `UserId` bigint NOT NULL,
        `status` longtext CHARACTER SET utf8mb4 NOT NULL,
        `ammount` double NOT NULL,
        `created_at` datetime(6) NOT NULL,
        CONSTRAINT `PK_Withdrawals` PRIMARY KEY (`WithdrawalId`),
        CONSTRAINT `FK_Withdrawals_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `Users` (`UserId`) ON DELETE CASCADE
    ) CHARACTER SET=utf8mb4;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230919203335_initMig') THEN

    CREATE INDEX `IX_Invites_OrganizationId` ON `Invites` (`OrganizationId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230919203335_initMig') THEN

    CREATE INDEX `IX_Lunches_recieverId` ON `Lunches` (`recieverId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230919203335_initMig') THEN

    CREATE INDEX `IX_Lunches_senderId` ON `Lunches` (`senderId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230919203335_initMig') THEN

    CREATE INDEX `IX_Lunches_UserId` ON `Lunches` (`UserId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230919203335_initMig') THEN

    CREATE INDEX `IX_OrganizationWallets_OrganizationId` ON `OrganizationWallets` (`OrganizationId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230919203335_initMig') THEN

    CREATE INDEX `IX_Users_OrganizationId` ON `Users` (`OrganizationId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230919203335_initMig') THEN

    CREATE INDEX `IX_Withdrawals_UserId` ON `Withdrawals` (`UserId`);

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230919203335_initMig') THEN

    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20230919203335_initMig', '6.0.22');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

COMMIT;

START TRANSACTION;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230919203624_finalMig') THEN

    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20230919203624_finalMig', '6.0.22');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

COMMIT;

START TRANSACTION;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230920035502_last_migration') THEN

    ALTER TABLE `Lunches` DROP FOREIGN KEY `FK_Lunches_Users_UserId`;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230920035502_last_migration') THEN

    ALTER TABLE `Lunches` DROP INDEX `IX_Lunches_UserId`;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230920035502_last_migration') THEN

    ALTER TABLE `Lunches` DROP COLUMN `UserId`;

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

DROP PROCEDURE IF EXISTS MigrationsScript;
DELIMITER //
CREATE PROCEDURE MigrationsScript()
BEGIN
    IF NOT EXISTS(SELECT 1 FROM `__EFMigrationsHistory` WHERE `MigrationId` = '20230920035502_last_migration') THEN

    INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
    VALUES ('20230920035502_last_migration', '6.0.22');

    END IF;
END //
DELIMITER ;
CALL MigrationsScript();
DROP PROCEDURE MigrationsScript;

COMMIT;


