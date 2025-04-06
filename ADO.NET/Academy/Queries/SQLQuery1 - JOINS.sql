USE VPD_311_Import;
GO

--SELECT 
--		direction_name,
--		COUNT(DISTINCT group_id) AS	N'Количество групп',
--		COUNT(stud_id)	AS	N'Количество студентов'
--FROM	Groups,Directions,Students
--WHERE	direction	= direction_id
--AND		[group]		= group_id
--GROUP BY direction_name;
--;

SELECT 
		direction_name,
		COUNT(DISTINCT group_id)	AS	N'Количество групп',
		COUNT(DISTINCT stud_id)		AS	N'Количество студентов'
FROM	Students
JOIN	Groups		ON	([group]	=	group_id)
RIGHT	JOIN	Directions	ON	(direction	=	direction_id)
GROUP BY direction_name;
;

--INNER JOIN (Default)
--OUTER JOIN:
--LEFT JOIN;  Левой считается дочерняя (зависимая) таблица, т.е. та, которая берет данные из другой таблицы.
--RIGHT JOIN; Правой считается родительская (независимая ) таблица, т.е. та, которая предоставляет данные.
--FULL  JOIN;