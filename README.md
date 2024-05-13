# **QuickService**

## **📗 목차**

<b>
  
- 📝 [개요]
- 🛠 [기술 및 도구]
- 📚 [라이브러리]
- 🔧 [기능구현]
  - [프로그램 등록]
  - [트레이 아이콘]
  - [프로그램 선택 및 실행]

</b>

## **📝 QuickService 개요**
![image](https://github.com/KR-JasonLane/QuickService/assets/98294800/bd957a03-247b-4fb6-8858-feef68a71349)

> **프로젝트 목적 :** C#, WPF연습 및 MVVM 패턴 학습
>
> **기획 및 제작 :** 이전석
>
> **주요 기능 :** 사용자가 사전에 프로그램을 등록하여 작업 중 키보드 단축기와 마우스 조작으로 간단하게 프로그램을 실행.
>
> **개발 환경 :** Windows 10, Visual Studio 2022 Community, .Net 8.0
>
> **문의 :** malbox5034@naver.com

<br/>

## **🛠 기술 및 도구**
![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)
![.NET](https://img.shields.io/badge/.NET-5C2D91?style=for-the-badge&logo=.net&logoColor=white)
![VS](https://img.shields.io/badge/Visual_Studio-5C2D91?style=for-the-badge&logo=visual%20studio&logoColor=whit)
![GitHub](https://img.shields.io/badge/GitHub-100000?style=for-the-badge&logo=github&logoColor=white)

<br />

## **📚 라이브러리**

|라이브러리|버전|비고|
|:---|---:|:---:|
|Newtonsoft.Json|13.0.3|사용자 환경설정|
|MaterialDesignThemes|4.9.0|UI 제작|
|CommunityToolkit.Mvvm|8.2.2|MVVM|
|Microsoft.Xaml.Behaviors.Wpf|1.1.77|MVVM|
|Microsoft.Extensions.DependencyInjection|8.0.0|의존성 주입|

<br/>

## **🔧기능 구현**

### **1. 프로그램 등록**

![2024-02-25 13 03 25](https://github.com/KR-JasonLane/QuickService/assets/98294800/07c403b1-549f-41ec-9b6b-140ce16add1c)
- Left, Top, Right, Bottom 영역 버튼을 클릭하여 사용자가 파일 경로를 지정.
- 사용자가 지정한 경로를 저장하고, 해당 파일의 아이콘을 읽어 ImageSource로 변환하여 View에 바인딩.

### **2. 트레이 아이콘**
![2024-02-25 13 10 11](https://github.com/KR-JasonLane/QuickService/assets/98294800/3c673286-9cd4-40c5-b9e0-680f599b893d)
- 우측 상단 Hide 버튼 클릭 시 창을 Hide상태로 변경.
- 작업표시줄에 트레이 아이콘 생성.
- Tray Icon 더블클릭 시 메인 윈도우 Open.
- Tray Icon을 우클릭 시 메뉴(Open, Close) 사용가능.

### **3. 프로그램 선택 및 실행**
![2024-02-25 13 27 01](https://github.com/KR-JasonLane/QuickService/assets/98294800/788fc1b5-9507-4ef9-a1e5-6e57793abf27)
- 조합키(Alt)와 마우스 좌측버튼을 눌러 프로그램 선택창 오픈.
- None Select 영역(가운데 X 영역)을 벗어나 실행할 프로그램의 영역 선택
- 마우스 좌측버튼 Up 시 선택된 영역의 프로그램 실행.
- None Select 영역에서 마우스 좌측버튼 Up / 조합키 Up 시 선택창 닫힘.

## **💾다운로드**

### [QuickService.zip](https://drive.google.com/file/d/1UgUS9h4n9krf0O9_7zet-yojzG36eQ-s/view?usp=sharing)

<br/>
<br/>
<br/>
<br/>
