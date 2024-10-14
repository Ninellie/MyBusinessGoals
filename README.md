# My Business Goals

My Business Goals - MVP приложения планировщика целей бизнеса.
Приложение сделано на движке Unity.

## Главное меню
В главном меню есть вертикальный бесконечный календарь. По клику на день будут показаны карточки с целями на этот день.

![Изображение](https://github.com/Ninellie/MyBusinessGoals/blob/master/GuHubReadmeResources/Main_Empty.png?raw=true "Главное меню")

## Сохранение целей
Скрипт GoalRepository.cs используется для сохранения целей в формате JSON в [Application.persistentDataPath](https://docs.unity3d.com/ScriptReference/Application-persistentDataPath.html). Конкретная папка зависит от платформы. Например, на Android это /storage/emulated/<userid>/Android/data/<packagename>/files

![Изображение](https://github.com/Ninellie/MyBusinessGoals/blob/master/GuHubReadmeResources/Add_goal_empty1.png?raw=true "Экран сохранения")

## Фильтрация и просмотр целей
На данном экране можно просмотреть все имеющиеся цели, а также отсортировать их по тегу и по подстроке в названии.

![Изображение](https://github.com/Ninellie/MyBusinessGoals/blob/master/GuHubReadmeResources/Search%20(find).png?raw=true "Экран просмотра всех целей")

## Новостная лента
![Изображение](https://github.com/Ninellie/MyBusinessGoals/blob/master/GuHubReadmeResources/Main_fill.png?raw=true "Экран новостей" )