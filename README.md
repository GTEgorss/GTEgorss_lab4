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
  clean-node Node1
  Server side:
  
![2022-05-27 08 04 14](https://user-images.githubusercontent.com/37060880/170632956-bcc635c1-c8ce-44b4-8ea5-7b358d3139a1.jpg)

![image_2022-05-27_08-07-58](https://user-images.githubusercontent.com/37060880/170633409-d0a10472-e396-4198-8bc7-964a4b747e20.png)

![image_2022-05-27_08-08-29](https://user-images.githubusercontent.com/37060880/170633462-9283a731-dcad-44b6-80ee-36451235c0b0.png)

![image_2022-05-27_08-09-05](https://user-images.githubusercontent.com/37060880/170633447-94f765a8-77a7-4ed9-a3d1-39a8cc68f9b1.png)

  Вывод: 
  1) Большую долю аллокаций занимают строки, а именно их конкатенация и метод ToString()
    Возможное решение: использование StringBuilder
  
  2) Чуть менее большую долю занимают массивы byte[], которые появляются при отправке сообщений нодам и приёме сообщений от них
    Возможное решение: как минимум, в некоторых случаях массив byte[] имеет известный и постоянный размер. В этом случае мы можем воспользоваться ArrayPool
  
  3) Char[] также занимает большую долю. Судя по информации из dotMemory, массив Char[] используется при чтении содержимого файлов.
    Возможное решение: затрудняюсь привести другой метод чтения
  
  
