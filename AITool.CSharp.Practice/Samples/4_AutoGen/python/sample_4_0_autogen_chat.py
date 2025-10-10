import asyncio
import os
from dotenv import load_dotenv
from AgentChat import AgentChat


async def main() -> None:
    # 載入 .env 檔案中的環境變數
    load_dotenv(encoding='utf-8-sig')

    # 從環境變數讀取 API Key
    api_key = os.getenv("OPENAI_API_KEY")

    if not api_key:
        raise ValueError("請設定 OPENAI_API_KEY 環境變數於 .env 檔案中!")

    await AgentChat(api_key)


if __name__ == "__main__":
    asyncio.run(main())
