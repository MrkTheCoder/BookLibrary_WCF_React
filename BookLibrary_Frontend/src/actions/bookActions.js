import axios from "axios";
import {
  BOOK_LIST_REQUEST,
  BOOK_LIST_SUCCESS,
  BOOK_LIST_FAIL,
} from "../constants/bookConstants";

export const listBooks = () => async (dispatch) => {
  try {
    dispatch({ type: BOOK_LIST_REQUEST });
    const { data } = await axios.get("http://localhost:51202/api/books");
    dispatch({ type: BOOK_LIST_SUCCESS, payload: data });
  } catch (error) {
    dispatch({
      type: BOOK_LIST_FAIL,
      payload: error,
    });
  }
};
