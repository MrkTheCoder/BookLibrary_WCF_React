export default function etagHandler(headers, data) {
  const Etag = headers["etag"];
  const ContentLength = headers["Content-Length"];

  if (Etag && ContentLength !== 0) {
    console.log(ContentLength, "CONTENT LEN");
    sessionStorage.setItem(Etag, JSON.stringify(data));
    return data;
  }

  const cachedData = JSON.parse(sessionStorage.getItem(Etag));
  console.log(cachedData, "triggred");

  return cachedData;
}
