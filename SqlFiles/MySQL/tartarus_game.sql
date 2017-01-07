-- MySQL Workbench Forward Engineering

SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='TRADITIONAL,ALLOW_INVALID_DATES';

-- -----------------------------------------------------
-- Schema Tartarus_Game
-- -----------------------------------------------------

-- -----------------------------------------------------
-- Schema Tartarus_Game
-- -----------------------------------------------------
CREATE SCHEMA IF NOT EXISTS `Tartarus_Game` DEFAULT CHARACTER SET utf8 ;
USE `Tartarus_Game` ;

-- -----------------------------------------------------
-- Table `Tartarus_Game`.`Character`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `Tartarus_Game`.`Character` (
  `character_id` INT NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(61) NULL,
  `account_id` INT NULL,
  `party_id` INT NULL,
  `guild_id` INT NULL,
  `slot` INT NULL,
  `x` INT NULL,
  `y` INT NULL,
  `layer` TINYINT NULL,
  `respawn_x` INT NULL,
  `respawn_y` INT NULL,
  `race` TINYINT NULL,
  `sex` TINYINT NULL,
  `level` INT NULL,
  `max_reached_level` INT NULL,
  `exp` BIGINT NULL,
  `last_decreased_exp` BIGINT NULL,
  `hp` INT NULL,
  `mp` INT NULL,
  `stamina` INT NULL,
  `havoc` INT NULL,
  `job` INT NULL,
  `job_level` INT NULL,
  `jp` INT NULL,
  `total_jp` INT NULL,
  `job_0` INT NULL,
  `job_1` INT NULL,
  `job_2` INT NULL,
  `jlevel_0` INT NULL,
  `jlevel_1` INT NULL,
  `jlevel_2` INT NULL,
  `huntaholic_point` INT NULL,
  `huntaholic_enter_count` INT NULL,
  `gold` BIGINT NULL,
  `chaos` INT NULL,
  `skin_color` INT UNSIGNED NULL,
  `hair` INT NULL,
  `face` INT NULL,
  `body` INT NULL,
  `hands` INT NULL,
  `feet` INT NULL,
  `texture_id` INT NULL,
  `belt_0` BIGINT NULL,
  `belt_1` BIGINT NULL,
  `belt_2` BIGINT NULL,
  `belt_3` BIGINT NULL,
  `belt_4` BIGINT NULL,
  `belt_5` BIGINT NULL,
  `summon_0` INT NULL,
  `summon_1` INT NULL,
  `summon_2` INT NULL,
  `summon_3` INT NULL,
  `summon_4` INT NULL,
  `summon_5` INT NULL,
  `main_summon` INT NULL,
  `sub_summon` INT NULL,
  `remain_summon_time` INT NULL,
  `pet` INT NULL,
  `guild_block_time` BIGINT NULL,
  `create_time` DATETIME NULL,
  `login_time` DATETIME NULL,
  `name_changed` INT NULL,
  `client_info` VARCHAR(4096) NULL,
  PRIMARY KEY (`character_id`))
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `Tartarus_Game`.`Item`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `Tartarus_Game`.`Item` (
  `item_id` BIGINT NOT NULL AUTO_INCREMENT,
  `character_id` INT NOT NULL,
  `account_id` INT NULL,
  `summon_id` INT NULL,
  `auction_id` INT NULL,
  `keeping_id` INT NULL,
  `idx` INT NULL,
  `code` INT NULL,
  `amount` BIGINT NULL,
  `level` INT NULL,
  `enhance` INT NULL,
  `durability` INT NULL,
  `endurance` INT NULL,
  `flag` INT NULL,
  `pos` INT NULL,
  `socket_0` INT NULL,
  `socket_1` INT NULL,
  `socket_2` INT NULL,
  `socket_3` INT NULL,
  `remain_time` INT NULL,
  `elemental_effect_type` TINYINT NULL,
  `elemental_effect_expire_time` DATETIME NULL,
  `elemental_effect_attack_point` INT NULL,
  `elemental_effect_magic_point` INT NULL,
  `create_time` DATETIME NULL,
  `update_time` DATETIME NULL,
  PRIMARY KEY (`item_id`),
  INDEX `fk_Item_Character_idx` (`character_id` ASC),
  CONSTRAINT `fk_Item_Character`
    FOREIGN KEY (`character_id`)
    REFERENCES `Tartarus_Game`.`Character` (`character_id`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB;

USE `Tartarus_Game` ;

-- -----------------------------------------------------
-- procedure usp_Lobby_GetCharacterList
-- -----------------------------------------------------

DELIMITER $$
USE `Tartarus_Game`$$
CREATE PROCEDURE `usp_Lobby_GetCharacterList` (IN accountId int)
BEGIN
	SELECT
		`character_id`,	/* 0 */
		`name`,			/* 1 */
		`race`,			/* 2 */
		`sex`,			/* 3 */
		`level`,		/* 4 */
		`exp`,			/* 5 */
		`hp`,			/* 6 */
		`mp`,			/* 7 */
		`job`,			/* 8 */
		`job_level`,	/* 9 */
		`skin_color`,	/* 10 */
		`hair`,			/* 11 */
		`face`,			/* 12 */
		`body`,			/* 13 */
		`hands`,		/* 14 */
		`feet`,			/* 15 */
		`texture_id`,	/* 16 */	
		`create_time`	/* 17 */
	FROM
		`Character`
	WHERE
		`account_id` = accountId;
END$$

DELIMITER ;

-- -----------------------------------------------------
-- procedure usp_Lobby_GetCharacterEquipment
-- -----------------------------------------------------

DELIMITER $$
USE `Tartarus_Game`$$
CREATE PROCEDURE `usp_Lobby_GetCharacterEquipment` (IN characterId BIGINT)
BEGIN
	SELECT
		`pos`, 					/* 0 */
		`code`,					/* 1 */
		`level`,				/* 2 */
		`enhance`,				/* 3 */
		`elemental_effect_type`	/* 4 */
	FROM
		`Item`
	WHERE
		`character_id` = characterId
        AND `pos` > -1
	ORDER BY
		`pos` ASC;
END$$

DELIMITER ;

SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;
