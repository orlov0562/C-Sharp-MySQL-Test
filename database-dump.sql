CREATE DATABASE IF NOT EXISTS mysqltest;

CREATE TABLE IF NOT EXISTS `test` (
`id` int(11) NOT NULL,
  `text` text NOT NULL
) ENGINE=InnoDB  DEFAULT CHARSET=utf8 AUTO_INCREMENT=14 ;

INSERT INTO `test` (`id`, `text`) VALUES
(1, 'Hello world'),
(3, 'Hello world 2'),
(4, 'Hello world 3'),
(6, 'Hello world 4'),
(7, 'Hello world 5'),
(9, 'Hello world 6'),
(13, 'Hello world 7');

ALTER TABLE `test` ADD PRIMARY KEY (`id`);

ALTER TABLE `test` 
MODIFY `id` int(11) NOT NULL AUTO_INCREMENT,AUTO_INCREMENT=14;