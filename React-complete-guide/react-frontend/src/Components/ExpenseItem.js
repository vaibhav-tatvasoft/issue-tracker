import React, { useEffect, useState } from "react";
import "./ExpenseItem.css";

function ExpenseItem(props) {
  const formattedDate = props.date.toLocaleDateString();
  const [newtitle, setTitle] = useState(props.name);

  useEffect(() => {
    console.log("title has changed!");
  }, [newtitle]);

  function onClickHandler() {
    console.log("clicked!!!!");
    console.log(newtitle);
    setTitle((prevState) =>
      prevState == props.name ? "UpdtaedTitle" : props.name
    );
    console.log(newtitle);
  }

  return (
    <div className="expense-item">
      <div>{formattedDate}</div>
      <div className="expense-item__desciption">
        <h2 onChange={() => console.log("onChnagetriggered!")}>{newtitle}</h2>
        <div className="expense-item__price">${props.amount}</div>
        <button onClick={onClickHandler}>submit</button>
      </div>
    </div>
  );
}

export default ExpenseItem;
