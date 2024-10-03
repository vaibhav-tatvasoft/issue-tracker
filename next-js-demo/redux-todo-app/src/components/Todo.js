import { useState, useEffect } from "react";
import { useDispatch, useSelector } from "react-redux";
import { ADD_TODO, EDIT_TODO, REMOVE_TODO } from "../actions";
import './Todo.css'

const Todo = () => {
  const [text, setText] = useState("");
  const dispatch = useDispatch();
  const onSubmitHandler = () => {
    dispatch(ADD_TODO(text));
  };
  const onRemoveHandler = (id) =>{
    dispatch(REMOVE_TODO(id))
  }
  const onEditHandler =(id, text) =>{
    dispatch(EDIT_TODO(id, text))
  }
  const todos = useSelector((state) => state.todos.list);
  useEffect(() => {
    console.log(todos);
  }, [todos]);

  return (
    <div className="todo">
      <form>
        <input
          type="text"
          placeholder="write something"
          value={text}
          onChange={(event) => {
            setText(event.target.value);
          }}
        />
        <button type="button" onClick={onSubmitHandler}>
          submit
        </button>
      </form>
      <div className="list-todo">
        {todos.length > 0 ? (
          todos.map((todo) => (
            <div key={todo.id} className="todo-item">
              <span>{todo.data}</span>
              <button onClick={() => onRemoveHandler(todo.id)}>remove</button>
              <button onClick={() => onEditHandler(todo.id, text)}>Edit</button>
            </div>
          ))
        ) : (
          <p>No todos available</p>
        )}
      </div>
    </div>
  );
};

export default Todo;
