# GooseAI
AI integration mod for Desktop goose using any AI provider (i think any)

![image1](https://raw.githubusercontent.com/Vexicle/GooseAI/refs/heads/main/Images/image1.png)

![image3](https://raw.githubusercontent.com/Vexicle/GooseAI/refs/heads/main/Images/image3.png)

images are a little out of date; will update soon

## How to install
To install, download the latest release and extract the folder to DesktopGoose/Mods, make sure the GooseAI folder is inside the goose desktop's mods folder.

Then, you need to configure the mod using the json file that should be in the GooseAI folder

Choose an AI provider; I recommend mistral because they give away free API usage (thank you mistral) or you can use openAI or use a local AI. Any AI provider that has a openai similar API should work.

the json examples below show usage with the mistral API.
### 1. In the JSON file, replace **"key"** with your real API key that you got from whatever provider you've chosen:
```json
"api_key": "kr7ApdZkEZr063jC2lfg67zAEAV3PB6Zy",
```
make sure to leave a comma at the end to not break the json file

### 2. Replace **"endpoint-url"** to your provider's enpoint:
```json
"endpoint": "https://api.mistral.ai/v1/chat/completions",
```
### 3. Replace **"ai-model"** with your chosen AI model:
```json
"model": "mistral-tiny",
```

after this, you should be good to go! read below to modify some extra configs to tailor your goose to your needs.

## How to use:

click on the goose 3 times for a input prompt; the input prompt is buggy and you'll have to click on it to start typing into it. Goose also talks randomly after idling. Change the frequency using the config "chanceToSpeak"

(sorry i tried multiple solutions but i think its due to the goose grabbing your mouse; including low level input grabbing)

## Extra configs
theres extra configs you can play around with to configure your goose's behaviour. here are the few that are in the json config. more will be added soon?

### chanceToSpeak
when idling, the goose has a % to speak, **increasing this number will increase the frequency your goose pesters you.** Lowering it to 0 ***should*** remove the idle goose speaking functionality (untested claim)

default:
```json
"chanceToSpeak": 0.05,
```
### systemPrompt
this flag defines your goose's behaviour, here you can **write instructions for your goose**, to reply in a certain fashion like "keep sentences short" or "only talk in quacks and emojis". some AI models forget this prompt for some reason

default:
```json
"systemPrompt": "You are a helpful goose. Additionally, you really want bread."
```
### bubbleText
this replaces the text you get in the prompt to anything you'd like

default:
```json
"bubbleText": "Need help? Or want to talk?"
```
### bubbleEnterText
this **replaces the text you'd press when you want to submit a text to the goose;** remember when you've got the prompt focused you can press the enter button to submit the text without needing to press the enter or ok button on the prompt window.

default:
```json
"bubbleEnterText": "Enter"
```

## How to build
You need visual studio with the .dotnet extension, it should work without needing to look for extra sdks. If you have to install something it should tell you

download the source code, also download desktop goose, delete the defaultMod folder at **"FOR MOD-MAKERS\GooseMod_DefaultSolution\DefaultMod"**, extract the source code, rename it to DefaultMod. **ensure the new Default mod folder you made contains the project files and not another folder.** In retrospect i probably should have uploaded the entire solution. (Will do in the future)

build as regular, and to use you'll have to **move the GooseAI.dll and newtonsoft.json.dll to Mods/GooseAI/**, also you'll have to create a **new file named "gooseai.json"**, you can use one from the release or you can paste this in:
```json
{
  "api_key": "key",
  "endpoint": "endpoint-url",
  "model": "ai-model",
  "chanceToSpeak": 0.05,
  "systemPrompt": "You are a goose. Additionally, you really want bread.",
  "bubbleText": "Need help? Or want to talk?",
  "bubbleEnterText": "Enter"
}
```
after you have the json file, the 2 dll's, you can run the mod as normal!
I haven't tested whether newtonsoft DLL can be simply left out of the mod folder, lmk in issues if it's required or not