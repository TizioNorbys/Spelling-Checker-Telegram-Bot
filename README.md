# Spelling Checker Telegram Bot
A **.NET** **Telegram Bot** for detecting any spelling mistakes in a text. The app uses **long polling** to receive realtime updates from the server. 
Communication with **Telegram Bot API** is handled by [Telegram.Bot](https://github.com/TelegramBots/Telegram.Bot) client library.
Checking and correction functionalities are provided by [Language Tool](https://languagetool.org/) API. 
\
\
Available [here](https://t.me/check_spell_bot).

## Features
- Any text message received is automatically analyzed
- The bot displays descriptions and possible corrections for every mistake it finds
- Suggested replacements are shown as inline buttons below mistake description messages
- Corrected texts are sent whenever the user selects a replacement for a mistake
- Users can select their preferred language, or let the bot try to guess it
- More than 50 languages and variants are supported

## Commands
- _/languages_: show a list of supported languages and variants
- _/setlanguage_: set the user's preferred languages — the command shows a set of inline buttons, one for every language or variant
