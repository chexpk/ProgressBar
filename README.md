# ProgressBar

[//]: # (![Screenshot]&#40;path/to/screenshot.png&#41;)

Реализована сцена с двумя экранами:
- экран MainMenu имеет список наград ожидающих получение и кнопку перехода к экрану ProgressBar;
- экран ProgressBar содержит список наград, шкалу прогресса, кнопку увеличения прогресса, копку возврата в MainMenu;

## Описание
- Unity 2022.3.14f1
- Запуск со сцены `BootStrap`

## Сторонние плагины
- [Zenject](https://github.com/modesttree/Zenject)
- [UniTask](https://github.com/Cysharp/UniTask) (добавлен ссылкой на git репозиторий)

## Использование
- Настройки значений начисления прогресса вынесены в SO в папке `/Data`
- Сохранение осуществляется в PlayerPrefs. Для сброса сохранения перейти `Edit -> Clear All PlayerPrefs`
