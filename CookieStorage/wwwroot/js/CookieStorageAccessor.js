export function get(key)
{
    return document.cookie.split("; ")
        .find((row) => row.startsWith(key + "="))
        ?.split("=")[1];
}

export function set(key, value)
{
    document.cookie = `${key}=${value}`;
}
