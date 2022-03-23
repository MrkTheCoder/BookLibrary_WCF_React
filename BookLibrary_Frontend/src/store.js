import { createStore, combineReducers, applyMiddleware } from "redux";
import thunk from "redux-thunk";
import { composeWithDevTools } from "redux-devtools-extension";
import { bookListReducer } from "./reducers/bookReducers";

const reducer = combineReducers({
  bookList: bookListReducer,
});

const initialState = {};

const middlewear = [thunk];

const store = createStore(
  reducer,
  initialState,
  composeWithDevTools(applyMiddleware(...middlewear))
);

export default store;
