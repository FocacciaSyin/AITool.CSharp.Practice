import asyncio
import os
from dotenv import load_dotenv
from my_autogen_agent_chat import my_agent_chat
from langfuse import Langfuse
import openlit


async def main() -> None:
    # 載入 .env 檔案中的環境變數
    load_dotenv(encoding='utf-8-sig')

    # 從環境變數讀取 API Key
    api_key = os.getenv("OPENAI_API_KEY")

    if not api_key:
        raise ValueError("請設定 OPENAI_API_KEY 環境變數於 .env 檔案中!")

    # [Langfuse Sample] 
    # Start
    # Filter out Autogen OpenTelemetryspans
    langfuse = Langfuse(
        blocked_instrumentation_scopes=["autogen SingleThreadedAgentRuntime"]
    )

    # Verify connection
    if langfuse.auth_check():
        print("Langfuse client is authenticated and ready!")
    else:
        print("Authentication failed. Please check your credentials and host.")

    openlit.init(tracer=langfuse._otel_tracer, disable_batch=True)
    # End

    await my_agent_chat(api_key, "gpt-4o-mini")


if __name__ == "__main__":
    asyncio.run(main())
