# Rule-Engine
A WPF application which is able to let user create multiple rules on stream data and will validate data against rules.

#### Briefly describe the conceptual approach you chose! What are the trade-offs?
WPF applicaion was choosen because its come with all inbuilt controls which helps me create solution quickly. When you are using build in framework of WPF you need not to worry about taking inputs as well as how you will format output. But WPF comes with some limitations, Like: its based on window OS which is non-portable to any other OS directly. 


#### What's the runtime performance? What is the complexity? Where are the bottlenecks?
I have tried to use best inbuild library so that it can built and run efficiently. Complexity of program is O(NxM) where N is number of test case and M is number of matching rule. Due to time constraint designing the solution was the main effort.

#### If you had more time, what improvements would you make, and in what order of priority?
Many things which can be improved with more time:
* Complexity of program can be improve. I need some more clarification and live enviroment to do this.
* Can be implemented a way to edit an existing rule
* Can be implemented a way to load an existing rule database
* Can be implemented a way that user can save existing rule as separate document
