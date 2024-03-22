![image](https://github.com/PrashaltGames/Unity-ConversationGraph/assets/58623243/822015b0-d29d-4f18-91f5-403e8071fa52)

# ConversationGraph; Node Based Script Engine for Unity.

## Why ConversationGraph?
- Intuitive!
- Great degree of freedom!
- Strong Performance on Runtime **maybe**!

## Set up
### Requirement
- Unity 2022 or later.
- UniTask is installed.

### Install
1. Open the Package Manager from Window > Package Manager
2. "+" button > Add package from git URL
3. Enter the following
     - `https://github.com/PrashaltGames/Unity-ConversationGraph.git?path=/Assets/ConversationGraph`


## Node Editor
Create Conversation with Node Editor.
### Open Editor
1. Make `Conversation Graph Asset` in ProjectWindow.
2. Double click `Conversation Graph Asset` to open Node Editor.

![image](https://github.com/PrashaltGames/Unity-ConversationGraph/assets/58623243/7ea800df-d5c1-468a-af62-7ce3639e67a0)

### Create Conversation
1. Right click to open menu.
2. Click `Create Node` to create a new node.
3. Set your Properties Asset in ConversationAsset; SubAsset of ConversationGraphAsset.

![image](https://github.com/PrashaltGames/Unity-ConversationGraph/assets/58623243/0910199a-2274-4b2e-a776-c27246a5dcf3)

## Runtime
### How to Play Conversation
Use Template.
1. Right click in Hierarchy window.
2. Create the template prefab in ConversationGraph.
3. Set your ConversationAsset; SubAsset of ConversationGraphAsset, in `ConversationSystem` component.
4. Excute `ConversationSystem.StartConversation()` by your C# Script.

![image](https://github.com/PrashaltGames/Unity-ConversationGraph/assets/58623243/333f0c6c-a64f-4c86-9965-7b01d7ff5557)

Use your custom.
1. Attach the `ConversationSystem` component at Your gameobject.
2. Set your ConversationAsset; SubAsset of ConversationGraphAsset, in `ConversationSystem` component.
3. Excute `ConversationSystem.StartConversation()` by your C# Script.

## Default Nodes
### Start Node
Entry point of Conversation.

Properties
- Title
![image](https://github.com/PrashaltGames/Unity-ConversationGraph/assets/58623243/f674bc0f-d5fe-4b15-8c31-1ca63db8c363)

### End Node
End of Conversation.
Warning: This node don't have function. So, you don't have to use this.

## Basic Nodes
### Narrator Node
Basic Conversation Node for Narrator.

Properties
- Messages
- TextAnimation
![image](https://github.com/PrashaltGames/Unity-ConversationGraph/assets/58623243/bc1417a5-540a-4498-8b8e-77c50b00387a)

### Speaker Node
Basic Conversation Node for Speaker.

Properties
- Speaker Name
- Messages
- Text Animation

### Select Node
Basic Conversation Node for Options. Conversation enable it to branch by select.

Properties
- Options

## Scriptable Nodes
### Scriptable Conversation
It excute your ccript on arrive.

### Scriptable Branch
It branch conversation by your script on arrive.

## How to create script for Scriptable Node
1. Create C# Script.
2. Implement `IScriptableConversation` or `IScriptableBranch` in your script.
3. Choise your script in `Scriptable Conversation Node` or `Scriptable Branch Node` on Node Editor.
  
## License
### Conversation Graph
[LICENSE](https://github.com/PrashaltGames/Unity-ConversationGraph/blob/main/LICENSE)

### Dependencies Packages
**UniTask**

The MIT License (MIT)

Copyright (c) 2019 Yoshifumi Kawai / Cysharp, Inc.

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.

**Unity-SerializeReferenceExtensions**

MIT License

Copyright (c) 2021 Hiroya Aramaki

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
