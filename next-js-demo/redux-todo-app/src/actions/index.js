export const ADD_TODO = (data) => {

    return {
    type: 'ADD_TODO',
    payload: {
        id: new Date().toISOString(),
        data: data
    }}
}

export const REMOVE_TODO = (id) =>{
    return {
        type: 'REMOVE_TODO',
        payload: {
            id: id
        }
    }
}

export const EDIT_TODO = (id, newData) =>{
    return{
        type: 'EDIT_TODO',
        payload: {
            id: id,
            data: newData
        }
    }
}