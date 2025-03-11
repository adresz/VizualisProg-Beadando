#include <iostream>
#include <string>

std::string reverseNumber(int num) {
    std::string str = std::to_string(num);
    bool isNegative = (num < 0);
    if (isNegative) {
        str = str.substr(1);  // Remove the negative sign for now
    }

    __asm {
        // Reverse the digits in the string
        lea esi, str[0]     // Load the address of the string
        lea edi, str[0] + strlen(str) - 1 // Load the address of the last character

        reverse_loop:
        mov al, [esi]       // Load the character at the start
            mov bl, [edi]       // Load the character at the end
                mov[edi], al       // Swap the characters
                    mov[esi], bl
                    inc esi             // Move to the next character
                    dec edi             // Move to the previous character
                    cmp esi, edi        // Check if pointers have crossed
                    jl reverse_loop     // If not, continue reversing
    }

    if (isNegative) {
        str.insert(str.begin(), '-');  // Re-add the negative sign
    }

    return str;
}

int main() {
    int num;
    std::cout << "Adj meg egy szamot: ";
    std::cin >> num;

    std::string reversed = reverseNumber(num);

    std::cout << "A megfordított szám: " << reversed << std::endl;

    return 0;
}
