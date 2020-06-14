# TraineeshipForum
To Run in Production Mode:

Lauch Visual Studio 2019 <br>
File -> Open Project <br>
Locate Downloaded Project<br>
Build -> Publish <br>
Choose Folder <br>
Locate your Folder <br>
Click Publish <br>
(Windows 10) Type in 'Type here to search': "appwiz.cpl" and Enter <br>
Click "Turn Windows Features on or off" <br>
Select "Internet Information Services" and Ok <br>
Open IIS Manager via Search bar <br>
Right click on "Sites" -> "Add Websites" <br>
Type sitename(<name>) and hostname(localhost) and select Physical path on drive (published area) <br>
In application Pool basic settings select "No managed Code" for app <br>
Type in Browser "localhost" <br>


To Run on Your Local Machine (Development Mode): <br> <br>
Open TraineeshipForum.sln via Visual Studio 2019 <br>
Go to Controllers <br>
AdministrationController <br>
Mark line #12 with "//" at the beggining. Entire line should be in green by now<br>
Ctrl + F5 <br>
Register new User with Email Confirmation<br>
Log In <br>
Type in Browser "/administration/listroles" <br>
Create New Role with Name - "Admin" <br>
Click "Edit" <br>
"Add/Remove Users" <br>
Select your Email <br>
Click "Update" <br>
Click "Home" <br>
Remove "//" from line #12 in the AdministrationController <br>
Ctrl + S <br>
Now you have full access to the app






