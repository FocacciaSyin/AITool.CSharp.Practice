from agent_framework import ChatAgent
from agent_framework.openai import OpenAIChatClient
from agent_framework.devui import serve

def get_weather(location: str) -> str:
    """Get weather for a location."""
    return f"Weather in {location}: 72°F and sunny"


def chat_agent(api_key: str) -> None:
    agent = ChatAgent(
        name="WeatherAgent",
        chat_client=OpenAIChatClient(model_id="gpt-4o-mini", api_key=api_key),
        tools=[get_weather]
    )

    serve(entities=[agent], auto_open=True, tracing_enabled=True)
