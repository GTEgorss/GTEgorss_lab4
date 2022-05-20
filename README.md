# GTEgorss_lab3
Optimizing

Стуктура:
<img width="256" alt="scheme" src="https://user-images.githubusercontent.com/37060880/169499080-11a2bc88-b4da-4a48-b418-ae8f86d73bd1.png">

<u>TCP Server:</u>
  • Node info: name, ip, port, size, files stored
  • AddNode();
  • AddFile();
  • RemoveFile();
  • CleanNode();
  
<u>TCP Node:</u>
  • Name, ip, port, base path (root folder), List<string> filePaths
  • AddFile();
  • RemoveFile();
  • CleanNode();
  • GetRelativePaths();


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
