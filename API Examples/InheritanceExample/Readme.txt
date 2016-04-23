Creating a base class for your Callouts allows you to manage common elements all in one place. Your base class stores properties and functions that are common to ALL of your callouts.

In the callout example class, you'll notice that you need to retrieve the suspect from the List each time you want to use it in a function.

This seems like extra legwork, but it keeps your code more organized and efficient. Plus, it becomes less work as you develop more callouts, as your CalloutBase class is disposing objects for you automatically.

This code was taken from the source code of Code 3 Callouts, but was converted from VB.NET into C#.

If there are any errors, please create an bug on the Issues page, and assign it to me.

Luke, I borrowed some of your chase callout example code. Hope you don't mind. ;)