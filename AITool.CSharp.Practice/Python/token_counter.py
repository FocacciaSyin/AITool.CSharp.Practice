import tiktoken

def count_tokens(text: str, encoding_name: str) -> int:
    """計算給定文字的 token 數量"""
    encoding = tiktoken.get_encoding(encoding_name)
    tokens = encoding.encode(text)
    return len(tokens)

def get_supported_encodings() -> list[str]:
    """返回支援的編碼列表"""
    return ["cl100k_base", "p50k_base", "r50k_base"]

def get_encoding_names() -> dict[str, str]:
    """返回編碼名稱映射"""
    return {
        "cl100k_base": "GPT-4",
        "p50k_base": "GPT-3.5-turbo",
        "r50k_base": "GPT-3"
    }
