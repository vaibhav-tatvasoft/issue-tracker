import { ADD_TODO } from "../actions";

const initialState = {
  list: [],
};

const TodoReducer = (state = initialState, actions) => {
  switch (actions.type) {
    case 'ADD_TODO':
      return {
        list: [...state.list, actions.payload],
      };

    case 'REMOVE_TODO':
        return {
            list: state.list.filter((todo) =>todo.id !== actions.payload.id)
        }

    case 'EDIT_TODO':
        return {
          list: state.list.map((todo) =>{
            if(todo.id === actions.payload.id){
              return{
                data: actions.payload.data
              }
            }
            return todo
          })
        }
    default : return state;
  }
};

export default TodoReducer;
