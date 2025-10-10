from autogen_agentchat.agents import AssistantAgent
from autogen_ext.models.openai import OpenAIChatCompletionClient


async def AgentChat(api_key) -> None:
    model_client = OpenAIChatCompletionClient(
        model="gpt-4o-mini",
        api_key=api_key
    )
    agent = AssistantAgent("assistant", model_client=model_client)
    print(await agent.run(task="AutoGen 是甚麼東西??,使用繁體中文回答"))
    await model_client.close()
