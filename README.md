# WebApiTest
Тестовое задание

Задание было сделано в соответсвии с указанными требованиями

Для работы приложения, были добавлены пакеты EntityFrameworkCore.SqlServer, EntityFrameworkCore.Tools.
Для подключения Swagger были добавлены пакеты Swashbuckle.AspNetCore, Swashbuckle.AspNetCore.Swagger, Swashbuckle.AspNetCore.SwaggerGen, Swashbuckle.AspNetCore.SwaggerUI.

1. В классе Operations.cs содержаться основные и вспомогательные сущности.
  Основные сущности были сделаны в соответсвии с требованиями.
  Так же для реализации фильтра дата конца, в сущность, которая хранит операции было добавлено поле End, оно обозначает дату окончания операции.
    Дополнительные сущности Analytics и Analytics1 хранят данные о аналитике, для 1 и 2 блока отдельно.

2. Собираем и запускаем проект.
  В открывшемся браузере, добавляем к значению localhost команду /swagger. После чего окткрывается интерфейс программы.

3. Необходимо сгенерировать данные для БД, для этого выполняем запросы api/articles/all ; api/operations/all ; api/contragents/all
  После подключения Swagger генерация большого объема данных стала занимать много времени, поэтому для ускорения процесса вместо 100000 скрипт заполняет 1000 операций.
  WebApi все еще поддерживает >100000 операций, просто это занимает много времени

4. WebApi готово к работе, около каждой функции есть ее описание, и так же пример необходимых и получаеммых данных для нее.
  
  Примечание: я так и не смог разобраться как отправлять массив строк для операций фильтров и аналитики(контрагенты и статьи).
  Я испробовал несколько методов, но WebApi все еще не принимало строки, поэтому внутри самой функции фильтров и аналитики где это нужно встроен специальный массив.
  Для изменения фильтра необходимо вписать туда новые значения. Их местоположение:
  [Фильтры]  (Filterscontollers.cs) 
	(Массив контрагентов строка 136)
  (Массив статей строка 70)
  [Аналитика]
  (Analyticscontoller.cs)
  /1
  Аналитка по контрагенту
  (Массив контрагентов строка 239)
  /2 
  Аналитика по операциям 
  (Массив статей строка 133)
  (Массив контрагентов строка 207)
  
  Так же не удалось полноценно реализовать наследование в сущности статей, поэтому они хранят название родительской статьи. Программа все еще корректно их обрабатывает.
  Так же там стоит специальная проверка на повтор, то есть если ввести дочернюю и родительскую статью, алгоритм не выведет 2 одинаковых дочерних статьи.
  
  Во время работы с аналитикой я сохранял данные в БД и выводил их все не очищая БД для проверки ошибок. Я решил их так оставить для того чтобы сверять результаты.
  В случае многократного вызова аналитики, запрошенный результат будет самый последний по id, остальные это результаты прошлых операций.
