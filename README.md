[Noobcoders Test-1.docx](https://github.com/a1right/NoobCoders.WebAPI/files/10127358/Noobcoders.Test-1.docx)



# Тестовое для NoobCoder`s

## Что делает программа своими словами

1. Запускает сервис WebAPI
2. Проверяет создана ли база данных.
3. Считывает .csv файл с исходными данными и загружает их в БД.
4. Проверяет существует ли индекс в эластике, если нет создаёт. Загружает туда данные из бд.
5. Принимает http запросы и в ответ на них выполняет действия, либо отдаёт информацию.



## Функционал:

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
  
  
