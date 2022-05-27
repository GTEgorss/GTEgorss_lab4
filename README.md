# GTEgorss_lab3

## Стуктура:
<img width="256" alt="scheme" src="https://user-images.githubusercontent.com/37060880/169501145-739a66ef-61a7-4a69-a645-3679cde751ce.png">


<ins>TCP Server:</ins>

  • Node info: name, ip, port, size, files stored
  
  • AddNode();
  
  • AddFile();
  
  • RemoveFile();
  
  • CleanNode();
  
  
<ins>TCP Node:</ins>

  • Name, ip, port, base path (root folder), List<string> filePaths
  
  • AddFile();
  
  • RemoveFile();
  
  • CleanNode();
  
  • GetRelativePaths();
  

 ## Начальный тест 

Тестирование программы:
3x add on server
![image_2022-05-20_09-50-10](https://user-images.githubusercontent.com/37060880/169482315-13d75dd1-0563-432c-9bb1-e6ac9afb821b.png)


3x add on node
![image_2022-05-20_10-03-53](https://user-images.githubusercontent.com/37060880/169482381-161c0338-5e60-41b0-8195-a8cefba3a47b.png)



3x add and 3x remove on server
![image_2022-05-20_10-08-31](https://user-images.githubusercontent.com/37060880/169482508-2ea009e8-1195-4112-8665-2aff1b426161.png)

3x add and 3x remove on node
![image_2022-05-20_10-10-50](https://user-images.githubusercontent.com/37060880/169482593-8361f456-c388-418b-a3cb-b91f3ea4cc78.png)


3x add and 3x clean on server
![image_2022-05-20_10-10-50](https://user-images.githubusercontent.com/37060880/169482675-685851be-c5f7-48b9-a488-b7dd56e353cf.png)


3x add and 3x clean on node
![image_2022-05-20_10-21-48](https://user-images.githubusercontent.com/37060880/169482744-2aa88154-e101-44d4-bec1-7b4a988974e0.png)


На всех запусках можно видеть большое количество string и byte[].
В связи с этим я добавил везде StringBuilder и Array.Pool там, где это было возможно.
  
  
  --------------------------------------------------------------------------------------------------------------------------------------------
  ## 27-05-2022
  
  3000x add-file to Node1
  
  Server side:
  
![2022-05-27 08 04 14](https://user-images.githubusercontent.com/37060880/170632956-bcc635c1-c8ce-44b4-8ea5-7b358d3139a1.jpg)

![image_2022-05-27_08-07-58](https://user-images.githubusercontent.com/37060880/170633409-d0a10472-e396-4198-8bc7-964a4b747e20.png)

![image_2022-05-27_08-08-29](https://user-images.githubusercontent.com/37060880/170633462-9283a731-dcad-44b6-80ee-36451235c0b0.png)

![image_2022-05-27_08-09-05](https://user-images.githubusercontent.com/37060880/170633447-94f765a8-77a7-4ed9-a3d1-39a8cc68f9b1.png)

  Вывод: 
  1) Большую долю аллокаций занимают строки, а именно их конкатенация и метод ToString()
    Возможное решение: использование StringBuilder
  
  2) Чуть менее большую долю занимают массивы byte[], которые появляются при отправке сообщений нодам
    Возможное решение: как минимум, в некоторых случаях массив byte[] имеет известный и постоянный размер. В этом случае мы можем воспользоваться ArrayPool
  
  3) Char[] также занимает большую долю. Судя по информации из dotMemory, массив Char[] используется при чтении содержимого файлов.
    Возможное решение: затрудняюсь привести другой метод чтения
  
  
  ### Проверка после введённых изменений:
  
![image_2022-05-27_08-49-49](https://user-images.githubusercontent.com/37060880/170639543-e67b429e-4ddb-4aa1-ab4b-215b445a6a1f.png)
  
  String builder:
  
  ![image_2022-05-27_08-53-48](https://user-images.githubusercontent.com/37060880/170639604-ba1e8709-a35b-4aa6-9d76-550b8d6b4fb5.png)

В данном случае мы видим, что вся наша расходуемая память просто ушла в ToString(). Я считаю, что тут дело в самом частном случае. StringBuilder не является эффективным решением, когда у нас не происходит множественных конкатенаций.
  
  
  Я также убрал все Console.WriteLine(), что тоже никак не появлияло на общих объём занимаемой памяти.
  
  
  ArrayPool:
  
  ![image_2022-05-27_08-57-56](https://user-images.githubusercontent.com/37060880/170639822-8cb547b6-bab0-4a13-92ae-b65ea6dd2760.png)
  
  В методе добавления файла невозможно было использование ArrayPool, ибо мы заранее не знаем требуемый размер массива
  
  
  
  Create Node1 and Node2; 1000x add-file to Node1; Clean-node Node1 (to Node2)
  
![image_2022-05-27_09-56-26](https://user-images.githubusercontent.com/37060880/170648360-2424d5ce-b5ad-494c-ae3f-67baf848ddb7.png)

![image_2022-05-27_10-01-22](https://user-images.githubusercontent.com/37060880/170648386-f96fc742-c95f-4a1a-96e0-cdaefd199bf7.png)

![image_2022-05-27_10-01-37](https://user-images.githubusercontent.com/37060880/170648404-dcf39fab-ddf5-4d6f-b321-87670586d30e.png)
  
![image_2022-05-27_10-01-43](https://user-images.githubusercontent.com/37060880/170648422-489ddb24-5b25-4bdc-b787-736b83f31279.png)
  
  Выводы:
  Здесь видно, что большую часть памяти занимают снова строки. Часть из всего этого занимается процессами добавления файлов (где-то под 30 Мб).
  Оставшиеся 70-75 Мб тратятся на Clean node.
  
  Также заметную часть занимают снова Char[] и byte[].
  
 Данные для Clean node:
![image_2022-05-27_10-09-16](https://user-images.githubusercontent.com/37060880/170649505-37fb0c9a-0211-4c2f-9b63-7f9e9e5b1161.png)
  
  Возможные решения:
  1) Для byte[] мы можем попробовать использовать ArrayPool, так как мы имеем массив постоянной длине в методе Clean node
  2) Мне кажется, что в данном случае char[] и string связаны. Следовательно, если мы уменьшим количество строк, то и, возможно, уменьшим количество char[]
  
  ![image_2022-05-27_11-50-05](https://user-images.githubusercontent.com/37060880/170665889-bb878045-10e4-4def-ba85-6bf107d886f8.png)
  
  ![Uploading image_2022-05-27_11-16-42.png…]()
  
  Добавил ArrayPool для byte[], немного уменьшилось. Меньше 1 Мб
  
  Добавил ArrayPool для string[], переписал немного алгоритм. Ничего не вышло. Появилось много конкатенации
  
  

