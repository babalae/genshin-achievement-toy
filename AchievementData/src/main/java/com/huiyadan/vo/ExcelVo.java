package com.huiyadan.vo;

import io.github.biezhi.excel.plus.annotation.ExcelColumn;
import lombok.Getter;
import lombok.Setter;
import org.apache.commons.compress.utils.Lists;
import org.apache.commons.lang.StringUtils;

import java.util.List;

@Getter
@Setter
public class ExcelVo {

    @ExcelColumn(index = 1)
    private String id;
    @ExcelColumn(index = 2)
    private String ver;
    @ExcelColumn(index = 3)
    private String hidden;
    @ExcelColumn(index = 4)
    private String edition;
    @ExcelColumn(index = 5)
    private String name;
    @ExcelColumn(index = 6)
    private String desc;
    @ExcelColumn(index = 7)
    private String customType;
    @ExcelColumn(index = 8)
    private String rarity;
    @ExcelColumn(index = 9)
    private String reward;
    @ExcelColumn(index = 10)
    private String remark;
    @ExcelColumn(index = 11)
    private String neta;

    public static List<TargetVo> toTargetVo(List<ExcelVo> voList) {
        List<TargetVo> list = Lists.newArrayList();
        for (ExcelVo excelVo : voList) {
            if (StringUtils.isNotBlank(excelVo.getId())) {
                list.add(toTargetVo(excelVo));
            }
        }
        return list;
    }

    public static TargetVo toTargetVo(ExcelVo vo) {
        TargetVo targetVo = new TargetVo();
        targetVo.setId(vo.getId());
        targetVo.setVer(vo.getVer());
        targetVo.setHidden(vo.getHidden());
        targetVo.setEdition(vo.getEdition());
        targetVo.setName(vo.getName());
        targetVo.setDesc(vo.getDesc());
        targetVo.setCustomType(vo.getCustomType());
        targetVo.setRarity(vo.getRarity());
        targetVo.setReward(vo.getReward());
        targetVo.setRemark(vo.getRemark());
        targetVo.setNeta(vo.getNeta());
        return targetVo;
    }
}
