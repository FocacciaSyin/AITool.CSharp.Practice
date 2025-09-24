"""
簡單測試 token_counter.py 的 count_tokens 功能
"""

from token_counter import count_tokens


def test_count_tokens():
    """測試 count_tokens 函數"""
    
    print("測試 count_tokens 函數...")

    # 測試範例
    test_cases = [
        ("Hello world"),
        ("這是中文測試"),
        (""),  # 空字串
        ("AI"),  # 短文字 
    ]
    
    encoding = "cl100k_base"

    for text in test_cases:
        token_count = count_tokens(text, encoding)
        print(f"'{text}' 使用 {encoding} 編碼: {token_count} tokens")

def main():
    """主函數"""
    print("開始簡單驗證 token_counter.py...")

    test_count_tokens()

    print("\n測試完成！")


if __name__ == "__main__":
    main()
