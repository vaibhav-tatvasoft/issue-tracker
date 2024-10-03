import { call, put, takeEvery, takeLatest } from "redux-saga/effects";
import axios from "axios";

export const FETCH_USER_SAGA = 'FETCH_USERS_SAGA';
export const FETCH_USER_SUCCESS_SAGA = 'FETCH_USERS_SUCCESS_SAGA';
export const FETCH_USER_FAILURE_SAGA = 'FETCH_USERS_FAILURE_SAGA';

export const fetchUsersSaga = () => ({
    type: FETCH_USER_SAGA
});
export const fetchUsersSuccessSaga = (data) => ({
    type: FETCH_USER_SUCCESS_SAGA,
    payload: data
});
export const fetchUsersFailureSaga = (error) => ({
    type: FETCH_USER_FAILURE_SAGA,
    payload: error
});

function* fetchUsersWorkerSaga() {
    try{
        const {data} = yield call(axios.get, "https://jsonplaceholder.typicode.com/users");
        console.log("Fetched Data (Saga):", data);
        yield put(fetchUsersSuccessSaga(data));
    }
    catch(error){
        yield put(fetchUsersFailureSaga(error.message))
    }
}

export function* watchFetchUsersSaga(){
    console.log('Saga is watching for users/fetchUsersSaga');
    yield takeEvery('FETCH_USERS_SAGA', fetchUsersWorkerSaga);
}

const initialState = {
    data: [],
    loading: false,
    error: null,
  };
  
  export default function usersSagaReducer(state = initialState, action) {
    switch (action.type) {
      case 'FETCH_USERS_SAGA':
        return { ...state, loading: true, error: null };
      case 'FETCH_USERS_SUCCESS_SAGA':
        return { ...state, loading: false, data: action.payload };
      case 'FETCH_USERS_FAILURE_SAGA':
        return { ...state, loading: false, error: action.payload };
      default:
        return state;
    }
  }


