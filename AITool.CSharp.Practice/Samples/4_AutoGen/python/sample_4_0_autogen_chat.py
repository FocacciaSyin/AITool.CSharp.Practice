import asyncio
import os
from autogen_agentchat.agents import AssistantAgent
from autogen_ext.models.openai import OpenAIChatCompletionClient

async def main() -> None:
    # 從環境變數讀取 API Key
    # api_key = os.getenv("OPENAI_API_KEY")
    
    api_key = "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"
    
    if not api_key:
        raise ValueError(
            "請設定 OPENAI_API_KEY 環境變數於 .env !"
        )
    
    model_client = OpenAIChatCompletionClient(
        model="gpt-4o-mini",
        api_key=api_key
    )
    agent = AssistantAgent("assistant", model_client=model_client)
    print(await agent.run(task="AutoGen 是甚麼東西??,使用繁體中文回答"))
    await model_client.close()

if __name__ == "__main__":
    asyncio.run(main())
