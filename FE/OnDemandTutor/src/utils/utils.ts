import axios, { AxiosError } from 'axios'
import { HttpStatusCode } from '../constant/HttpStatusCode.enum'
import config from '../constant/config'
import userImage from '../assets/img/user.svg'

// do là token hết hạn chúng ta không nên chơi reload trang chún ta dùng cách này
export const LocalStrorageEventTarget = new EventTarget()

export function isAxiosError<T>(error: unknown): error is AxiosError<T> {
  return axios.isAxiosError(error as any)
}

//  đây là hàm dùng để check lỗi có phải 422
export function isAxiosUnprocessableEntityError<FromError>(
  error: unknown
): error is AxiosError<FromError> {
  return (
    isAxiosError(error) &&
    error.response?.status === HttpStatusCode.UnprocessableEntity
  )
}

//  cú phápn '-?' sẽ loại bỏ cái key opitonal (hande?)
// nó sẽ loai bỏ undefined ủa key optional
export type NoUndefinedField<T> = {
  [P in keyof T]-?: NoUndefinedField<NonNullable<T>[P]>
}

export const getAvatarUrl = (avatarName?: string) =>
  avatarName ? `${config.baseUrl}images/${avatarName}` : userImage

// Code xóa các ký tự đặc biệt trên bàn phím
const removeSpecialCharacter = (str: string) =>
  // eslint-disable-next-line no-useless-escape
  str.replace(
    /!|@|%|\^|\*|\(|\)|\+|\=|\<|\>|\?|\/|,|\.|\:|\;|\'|\"|\&|\#|\[|\]|~|\$|_|`|-|{|}|\||\\/g,
    ''
  )

//chuyển path
export const generateNameId = ({
  title,
  idRequest
}: {
  title: string
  idRequest: string
}) => {
  return removeSpecialCharacter(title).replace(/\s/g, '-') + `-i-${idRequest}`
}

export const getIdFromNameId = (nameId: string) => {
  const arr = nameId.split('-i-')
  return arr[arr.length - 1] //vị trí cuối
}
