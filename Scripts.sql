INSERT INTO Продажи 
([Номер товара], [Количество], [Дата], [Время], [Номер филиала], [Номер продавца] )
Values 
(idProduct,selled, 'getDate()', 'getTime()',numberBranche,numberSellers);

Update Лекарства 
set [Количество товара на складе] = newCount
where [Номер лекарства] =idProduct;

Update Лекарства 
set [Количество товара на складе] = newCount,
[Расположение товара на складе] = 'textBox9.Text'  
where [Номер лекарства] = idProduct;
Поиск лекарства по номеру:
SELECT Лекарства.[Количество товара на складе], Лекарства.[Расположение товара на складе] 
FROM Лекарства
where Лекарства.[Номер лекарства] = id;

SELECT Лекарства.Название,SUM (Продажи.Количество),
 SUM (Продажи.Количество*Лекарства.Цена)
FROM Лекарства 
INNER JOIN Продажи ON Лекарства.[Код] = Продажи.[Номер товара]
GROUP BY Лекарства.Название
ORDER BY SUM (Продажи.Количество*Лекарства.Цена) DESC;

SELECT Филиалы.Код, Филиалы.Название, SUM(Продажи.Количество), SUM(Лекарства.Цена*Продажи.Количество)
FROM Филиалы 
INNER JOIN (Лекарства 
    INNER JOIN Продажи ON Лекарства.[Код] = Продажи.[Номер товара]
           ) ON Филиалы.[Код] = Продажи.[Номер продавца]
GROUP BY Филиалы.Код, Филиалы.Название
ORDER BY SUM(Лекарства.Цена*Продажи.Количество) DESC;

SELECT Продажи.[Номер продавца],Продажи.[Номер филиала], 
SUM(Продажи.Количество), 
SUM(Продажи.Количество*Лекарства.Цена)
FROM Лекарства 
INNER JOIN Продажи ON Лекарства.[Код] = Продажи.[Номер товара]
GROUP BY Продажи.[Номер филиала], Продажи.[Номер продавца]
ORDER BY  SUM(Продажи.Количество*Лекарства.Цена) DESC;
