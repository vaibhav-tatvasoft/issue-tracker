import logo from "./logo.svg";
import "./App.css";

import ExpenseItem from "./Components/ExpenseItem.js";

function App() {
  const arr = [
    { date: new Date(2024, 12, 23), name: "vaibhav", amount: 34 },
    { date: new Date(2022, 10, 10), name: "jefw", amount: 45 },
    { date: new Date(2023, 6, 12), name: "iwoqjo", amount: 65 },
    { date: new Date(2016, 8, 3), name: "oirtrjoe", amount: 23 },
  ];

  return (
    <div className="App">
      <h1>This is App.js</h1>
      <div className="Expenses">
        <ExpenseItem
          date={arr[0].date}
          name={arr[0].name}
          amount={arr[0].amount}
        />
        <ExpenseItem
          date={arr[1].date}
          name={arr[1].name}
          amount={arr[1].amount}
        />
        <ExpenseItem
          date={arr[2].date}
          name={arr[2].name}
          amount={arr[2].amount}
        />
        <ExpenseItem
          date={arr[3].date}
          name={arr[3].name}
          amount={arr[3].amount}
        />
      </div>
    </div>
  );
}

export default App;
