[Noobcoders Test-1.docx](https://github.com/a1right/NoobCoders.WebAPI/files/10127358/Noobcoders.Test-1.docx)



# Тестовое для NoobCoder`s

## Что делает программа своими словами

1. Запускает сервис WebAPI
2. Проверяет создана ли база данных.
3. Считывает .csv файл с исходными данными и загружает их в БД.
4. Проверяет существует ли индекс в эластике, если нет создаёт. Загружает туда данные из бд.
5. Принимает http запросы и в ответ на них выполняет действия, либо отдаёт информацию.



## Функционал

**[GET]**
  * api/records/posts                 - получить список всех сообщений
  * api/records/post/{id}             - получить сообщение по значению id
  * api/records/posts/contains/{text} - получить 20 сообщений, содержащих строку {text}, отсортированных по дате создания
  * api/records/rubrics               - получить список всех рубрик
  * api/records/rubrics/{id}          - получить рубрику по значению id  
  
  **[DELETE]**
  + api/records/posts/{id}            - удалить сообщение по значению id
  + api/records/rubrics/{id}          - удалить рубрику по значению id
  
  ![image](https://user-images.githubusercontent.com/24682568/205158639-4979b521-bbe5-4b64-9e68-78a370bd37d9.png)
  
## Для работы приложения потребуется

  1. **MS SQL Server**
  2. **ElasticSearch**
  3. "CsvHelper" Version="30.0.1"  
  4. "Elastic.Apm.NetCoreAll" Version="1.18.0"  
  5. "NEST" Version="7.17.5"  
  6. "Swashbuckle.AspNetCore" Version="6.2.3"  
  7. Microsoft.EntityFrameworkCore" Version="7.0.0"  
  8. Microsoft.EntityFrameworkCore.Design Version="7.0.0"  
  9. Microsoft.EntityFrameworkCore.SqlServer Version="7.0.0"  
    
    
