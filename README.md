# TraineeshipForum
To Run in Production Mode(locally):

In your Solution Explorer select project and right-click to select Publish <br>
Select Folder and click Next <br>
Select "Folder Location" and Click Finish <br>
Click Publish <br><br>
Open IIS <br>
Right click on "Sites" -> "Add Website..." <br>
Select "Site name" and pick physical path equal to "Folder Location", dont forget to use free port <br>
Go to Application Pools <br>
Right click on Application Pool name, which is the same as your "-Site Name-" and select Advanced Settings <br>
Make Identity equal "Local System" in Process Model <br>
Right Click on "-Site Name-", select "Manage Application" and click "Browse" <br><br>
To make Email confirmation work you will need <a href="https://sendgrid.com/">SendGrid account</a> with verified email <br>
In profile menu "Settings" -> API Keys -> Create API Key <br>
Type in API key name, that will be used in "Manage User Secrets"( right click on Solution name) json file as SendGridUser field <br>
In "Restriction Access" choose Mail Send and Click Create & View <br>
Copy Api Key and paste it in secrets.json file in SendGridKey field <br>
In the end you will need to add, verified by SendGrid, email address in line 29 <br>
To verify go to Marketing -> Senders -> Create New Sender <br>







