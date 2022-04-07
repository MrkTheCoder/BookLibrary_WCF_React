import { createStore, combineReducers, applyMiddleware } from "redux";
import thunk from "redux-thunk";
import { composeWithDevTools } from "redux-devtools-extension";
import { bookListReducer, bookDetailsReducer } from "./reducers/bookReducers";
import {
  categoryListReducer,
  filtersReducer,
} from "./reducers/categoryReducers";
import { borrowerListReducer } from "./reducers/borrowersReducers";

const reducer = combineReducers({
  bookList: bookListReducer,
  bookDetails: bookDetailsReducer,

  categories: categoryListReducer,
  filters: filtersReducer,

  borrowers: borrowerListReducer,
});

const initialState = {};

const middlewear = [thunk];

const store = createStore(
  reducer,
  initialState,
  composeWithDevTools(applyMiddleware(...middlewear))
);

export default store;
