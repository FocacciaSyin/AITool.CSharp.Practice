import tiktoken

def count_tokens(text: str, encoding_name: str) -> int:
    """計算給定文字的 token 數量"""
    encoding = tiktoken.get_encoding(encoding_name)
    tokens = encoding.encode(text)
    return len(tokens)
