import { useState } from "react";
import { useMutation, useQuery } from "@tanstack/react-query";
import { adminAPI } from "../../../../api/admin.api";
import { Button, Modal, Table, TableColumnsType } from "antd";
import { toast } from "react-toastify";
import { complaintType } from "../../../../types/complaint.type";

export default function AdminViewComplaint() {
    const { data: complaintData, refetch } = useQuery({
        queryKey: ['complaintData'],
        queryFn: () => adminAPI.getComplaintList(),
    });
    const [searchText, setSearchText] = useState(''); // liên quan đến giá trị input vào search
    const [selectedRecord, setSelectedRecord] = useState<complaintType | null>(null);
    const [open, setOpen] = useState(false);

    const columns: TableColumnsType<complaintType> = [
        {
            title: 'Thông tin',
            dataIndex: 'description',
            width: 200,
            fixed: 'left',
        },
        {
            title: 'Tên học sinh',
            dataIndex: ['user', 'fullName'],
            width: 200,
        },
        {
            title: 'Tên gia sư',
            dataIndex: ['tutor', 'fullName'],
            width: 200,
        },
        {
            title: 'Xem chi tiết',
            dataIndex: 'action',
            className: 'TextAlign',
            fixed: 'right',
            width: 150,
            render: (text: string, record: complaintType) => (
                <div className='flex gap-1'>
                    <button
                        className='p-1 border border-red-500 rounded-lg hover:bg-red-500 active:bg-red-700'
                        onClick={() => showDetail(record)}
                    >
                        Chi tiết
                    </button>
                </div>
            ),
        },
    ];

    const showDetail = (record: complaintType) => {
        setSelectedRecord(record);
        setOpen(true);
    };

    const handleCancel = () => {
        setOpen(false);
        setSelectedRecord(null);
    };

    const onChange = () => {};

    const removeMutation = useMutation({
        mutationFn: (idAccount: string) => adminAPI.deleteAccount(idAccount),
        onSuccess: () => {
            toast.success('Đã xóa tài khoản');
            refetch(); // Gọi lại API để cập nhật lại danh sách yêu cầu
            setOpen(false);
        },
    });

    const handleDelete = () => {
        if (selectedRecord) {
            removeMutation.mutate(selectedRecord.tutor.id);
            console.log('id tutor nè ', selectedRecord.tutor.id);
        }
    };

    return (
        <>
            <div>
                <div className='text-left m-5'>Danh sách tố cáo của học sinh</div>
                <Table
                    className=''
                    columns={columns}
                    dataSource={complaintData?.data}
                    pagination={{ pageSize: 10 }}
                    onChange={onChange}
                    showSorterTooltip={{ target: 'sorter-icon' }}
                    scroll={{ x: 1300, y: 400 }}
                />
                <div>
                    <Modal
                        title='Thông tin chi tiết đơn tố cáo'
                        open={open}
                        onCancel={handleCancel}
                        footer={[
                            <Button key='back' onClick={handleDelete}>
                                Xóa tài khoản gia sư
                            </Button>,
                        ]}
                    >
                        {selectedRecord && (
                            <div>
                                <div className="flex justify-between mb-4">
                                    <div>
                                        <p className="font-bold text-blue-600 mb-2">Thông tin học sinh</p>
                                        <p className="font-semibold mb-1">Tên: <span className="font-normal text-gray-700">{selectedRecord.user.fullName}</span></p>
                                        <p className="font-semibold mb-1">Email: <span className="font-normal text-gray-700">{selectedRecord.user.email}</span></p>
                                        <p className="font-semibold mb-1">Giới tính: <span className="font-normal text-gray-700">{selectedRecord.user.gender}</span></p>
                                        <p className="font-semibold mb-1">Số điện thoại: <span className="font-normal text-gray-700">{selectedRecord.user.phone}</span></p>
                                    </div>
                                    <div>
                                        <p className="font-bold text-red-600 mb-2">Thông tin gia sư</p>
                                        <p className="font-semibold mb-1">Tên: <span className="font-normal text-gray-700">{selectedRecord.tutor.fullName}</span></p>
                                        <p className="font-semibold mb-1">Email: <span className="font-normal text-gray-700">{selectedRecord.tutor.email}</span></p>
                                        <p className="font-semibold mb-1">Giới tính: <span className="font-normal text-gray-700">{selectedRecord.tutor.gender}</span></p>
                                        <p className="font-semibold mb-1">Số điện thoại: <span className="font-normal text-gray-700">{selectedRecord.tutor.phone}</span></p>
                                    </div>
                                </div>
                                <div className="text-center mt-4">
                                    <p className="font-bold text-green-600 mb-2">Mô tả:</p>
                                    <p className="font-normal text-gray-700">{selectedRecord.description}</p>
                                </div>
                            </div>
                        )}
                    </Modal>
                </div>
            </div>
        </>
    );
}



