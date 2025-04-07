#!/usr/bin/env python
"""
Age Predictor
------------
This script asks the user for their name and age, 
then predicts their birth year based on the current date.
"""

import datetime

def calculate_birth_year(age):
    """Calculate approximate birth year based on age"""
    current_year = datetime.datetime.now().year
    birth_year = current_year - age
    return birth_year

def main():
    print("ðŸ‘‹ Welcome to the Age Predictor!")
    print("=" * 40)
    
    # Get user input
    name = input("Please enter your name: ")
    
    # Validate age input
    while True:
        try:
            age = int(input("Please enter your age: "))
            if age < 0 or age > 120:
                print("Please enter a valid age between 0 and 120.")
                continue
            break
        except ValueError:
            print("Please enter a numeric age.")
    
    # Calculate birth year
    birth_year = calculate_birth_year(age)
    
    # Display results
    print("\n" + "=" * 40)
    print(f"Hello, {name}!")
    print(f"Based on your age ({age}), you were likely born in or around {birth_year}.")
    
    # Add more birth date prediction details
    current_month = datetime.datetime.now().month
    current_day = datetime.datetime.now().day
    
    print("\nMore specifically:")
    print(f"If you've already had your birthday this year, you were born in {birth_year}.")
    print(f"If you haven't had your birthday yet this year, you were born in {birth_year-1}.")
    
    # Calculate possible birth date range
    earliest_date = datetime.date(birth_year-1, current_month, current_day + 1)
    latest_date = datetime.date(birth_year, current_month, current_day)
    
    print(f"\nYour birth date is likely between:")
    print(f"{earliest_date.strftime('%B %d, %Y')} and {latest_date.strftime('%B %d, %Y')}")
    
    print("\nThank you for using the Age Predictor!")

if __name__ == "__main__":
    main()
