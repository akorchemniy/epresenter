SELECT DISTINCT VERSION
FROM BIBLEVERSES

SELECT DISTINCT REFBOOK
FROM BIBLEVERSES

SELECT FIRST 10 *
FROM BIBLEVERSES AS PRI
WHERE PRI.VERSION = 'RST' AND PRI.REFBOOK = 'Psalms'

-- This makes no sense
SELECT *
FROM BIBLEVERSES AS B
WHERE B.REFBOOK = 'Psalms' AND B.CHAPTER = 23
ORDER BY B.REFBOOK, B.REFCHAPTER, B.REFVERSE, B.VERSION

-- From ePresenter. KJV, RST Psalms 23
SELECT *
FROM (BIBLEVERSES AS PRI LEFT JOIN BIBLEVERSES AS SEC
	ON PRI.BOOK = SEC.REFBOOK AND PRI.CHAPTER = SEC.REFCHAPTER AND PRI.VERSE = SEC.REFVERSE)
WHERE PRI.VERSION = 'KJV' AND SEC.VERSION = 'RST' AND PRI.REFBOOK = 'Psalms' AND PRI.REFCHAPTER = 23
ORDER BY PRI.REFBOOK, PRI.REFCHAPTER, PRI.REFVERSE, PRI.VERSE

-- From ePresenter. RST, KJV Psalms 22
SELECT *
FROM (BIBLEVERSES AS PRI LEFT JOIN BIBLEVERSES AS SEC
	ON PRI.BOOK = SEC.REFBOOK AND PRI.CHAPTER = SEC.REFCHAPTER AND PRI.VERSE = SEC.REFVERSE)
WHERE PRI.VERSION = 'RST' AND SEC.VERSION = 'KJV' AND PRI.REFBOOK = 'Psalms' AND PRI.REFCHAPTER = 22
ORDER BY PRI.REFBOOK, PRI.REFCHAPTER, PRI.REFVERSE, PRI.VERSE

-- From ePresenter. KJV, RST, UK Psalms 23
SELECT *
FROM (BIBLEVERSES AS PRI 
	LEFT JOIN BIBLEVERSES AS SEC
		ON PRI.BOOK = SEC.REFBOOK AND PRI.CHAPTER = SEC.REFCHAPTER AND PRI.VERSE = SEC.REFVERSE
	LEFT JOIN BIBLEVERSES AS TRI
		ON PRI.BOOK = TRI.REFBOOK AND PRI.CHAPTER = TRI.REFCHAPTER AND PRI.VERSE = TRI.REFVERSE)
WHERE PRI.VERSION = 'KJV' AND SEC.VERSION = 'RST' AND TRI.VERSION = 'UK'
	AND PRI.REFBOOK = 'Psalms' AND PRI.REFCHAPTER = 23
ORDER BY PRI.REFBOOK, PRI.REFCHAPTER, PRI.REFVERSE, PRI.VERSE

-- From ePresenter. RST, KJV, UK Psalms 22
SELECT PRI.REFBOOK, PRI.REFCHAPTER, PRI.REFVERSE, PRI.DATA, SEC.DATA, TRI.DATA
FROM (BIBLEVERSES AS PRI 
	LEFT JOIN BIBLEVERSES AS SEC
		ON PRI.BOOK = SEC.REFBOOK AND PRI.CHAPTER = SEC.REFCHAPTER AND PRI.VERSE = SEC.REFVERSE
	LEFT JOIN BIBLEVERSES AS TRI
		ON PRI.REFBOOK = TRI.REFBOOK AND PRI.REFCHAPTER = TRI.REFCHAPTER AND PRI.REFVERSE = TRI.REFVERSE)
WHERE PRI.VERSION = 'RST' AND PRI.REFBOOK = 'Psalms' AND PRI.REFCHAPTER = 22
	AND SEC.VERSION = 'KJV' AND TRI.VERSION = 'UK'
ORDER BY PRI.REFBOOK, PRI.REFCHAPTER, PRI.REFVERSE, PRI.VERSE

