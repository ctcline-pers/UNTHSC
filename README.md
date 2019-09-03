# UNTHSC
Coding Challenge for UNTHSC

Notes:
- I used migrations for all DB work, so there are no additional SQL scripts included
- For the Customer SystemRole, I was unsure of the intended purpose of that.  Since it was the weeked, I made an assumption that it was intended to be related to whatever system this would have been built for. So I stubbed it out with the built in Windows roles so it would be familiar. I look forward to hearing the desired implementation for this one
- I have only created the Admin user role and stubbed out to add my email address as the intial admin user with a generic password.  This is strictly to facilitate delivery and use of this sample app.  Were this in production, this could still be a valid option, but I would prefer to include a check to force a change of the admin password from the default before any customers could be added.
