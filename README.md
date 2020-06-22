# TraineeshipForum
To make Email confirmation work you will need <a href="https://sendgrid.com/">SendGrid account</a> with verified email <br>
In profile menu "Settings" -> API Keys -> Create API Key <br>
Type in API key name<br>
In "Restriction Access" choose Mail Send and Click Create & View <br>
Copy API Key and paste it in secrets.json file in SendGridKey field <br>
Complete the same with API key name by copiing it SendGridUser field<br>
In the end you will need to add, verified by SendGrid, email address in line 29 of IEmailSender.cs in Services-Interfaces<br>
To verify email go to Marketing -> Senders -> Create New Sender <br><br>
To Run in Production Mode(locally):

In your Solution Explorer select project and right-click to select Publish <br>
Select Folder and click Next <br>
Select "Folder Location" and Click Finish <br>
Click Publish <br>

Open IIS <br>
Right click on "Sites" -> "Add Website..." <br>
Select "Site name" and pick physical path equal to "Folder Location", dont forget to use free port <br>
Go to Application Pools <br>
Right click on Application Pool name, which is the same as your "-Site Name-" and select Advanced Settings <br>
Make Identity equal "Local System" in Process Model <br>
Right Click on "-Site Name-", select "Manage Application" and click "Browse" <br><br>

After successful registration you need to create and assign role "Admin" to a particular user<br>
It will allows you to create categories <br> 
Go to "Admin" Section on Top and Click "Create Role" with name "Admin" <br>
Click Edit and Add/Remove Users -> Assign registrated user to this role -> Click Update<br>
Now you can add Forum categories on Home Page <br>
To add restrictions to "Roles" area remove "//" in the Administration Controller





