import * as yup from 'yup'
//npm i -D yup if dont work
//  này là mình xài yup của react hook form để làm validate nha fen
//

// type Rules = {
//   // chỗ này là sao?
//   //  có nghĩa là:
//   // chúng ta định dạng được lấy từ RegisterOptions trong react hookform
//   // bên trong thằng RegisterOptions này có các key hỗ trợ cho việc input
//   // chúng ta lấy mấy key đó ra xài để tránh gõ sai
//   [key in 'email' | 'password' | 'confirm_password']?: RegisterOptions
// }

export const schemaResAT = yup.object({
  qualificationName: yup.string().required('Không được bỏ trống'),
  type: yup
    .string()
    .oneOf(['chứng chỉ', 'Bằng Cắp'], 'phải là Chứng Chỉ or bằng cấp')
    .required('Xếp loại bằng là bắt buộc'),
  field: yup.string().required('Không được bỏ trống'),
  experience: yup
    .number()
    .typeError('Chỉ được nhập số (năm giảng dạy, dạy thêm,...)')
    .required('Trường này là bắt buộc'),
  specializedSkills: yup.string().required('Không được bỏ trống'),
  imageQualification: yup.string().required('Ảnh còn thiếu')
})

// này là mình export cái schema (đinhj dạng lỗi) của mình ra để qua bên Input bắt lỗi
export type SchemaResAT = yup.InferType<typeof schemaResAT>
