import os
from dotenv import load_dotenv
from agent_chat_with_dev_ui import chat_agent


def main():
    # 載入 .env 檔案中的環境變數
    load_dotenv(encoding='utf-8-sig')

    # 從環境變數讀取 API Key
    api_key = os.getenv("OPENAI_API_KEY")

    chat_agent(api_key)


if __name__ == "__main__":
    main()
