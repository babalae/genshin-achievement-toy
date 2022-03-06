package com.huiyadan.vo;

import com.fasterxml.jackson.annotation.JsonIgnore;
import com.fasterxml.jackson.annotation.JsonInclude;
import io.github.biezhi.excel.plus.annotation.ExcelColumn;
import lombok.Getter;
import lombok.Setter;
import org.apache.commons.lang.StringUtils;

import java.util.List;

@JsonInclude(JsonInclude.Include.NON_NULL)
@Getter
@Setter
public class TargetVo {

    @ExcelColumn(index = 0, title = "id")
    private String id;

    @ExcelColumn(index = 1, title = "版本")
    private String ver;

    private String hidden;

    @ExcelColumn(index = 2, title = "特辑")
    private String edition;

    @ExcelColumn(index = 3, title = "名称")
    private String name;

    @ExcelColumn(index = 4, title = "描述")
    private String desc;

    private String customType;

    private String rarity;

    @ExcelColumn(index = 5, title = "原石")
    private String reward;

    private String remark;

    private String neta;

    /**
     * 存在上下级依存的成就集合
     */
    private List<String> levels;

    @JsonIgnore
    private boolean checked = false;

    @JsonIgnore
    @ExcelColumn(index = 6, title = "校验不一致的原因")
    private String checkReason = "";

    public boolean check(TargetVo other) {
        if (checked || other.checked) {
            throw new RuntimeException("有成就数据存在重复校验:" + id + "/" + other.id);
        }

        try {
            if (!name.equals(other.name)) {
                checkReason += "名称不一致;";
            }
            if (!ver.equals(other.ver)) {
                checkReason += "版本不一致;";
            }
            if (!edition.equals(other.edition)) {
                checkReason += "特辑不一致;";
            }
            if (!desc.equals(other.desc)) {
                checkReason += "描述不一致;";
            }
            if (reward == null || !reward.equals(other.reward)) {
                checkReason += "原石不一致;";
            }
        } catch (Exception e) {
            checkReason += "校验时出现异常;";
        }
        if (StringUtils.isBlank(checkReason)) {
            checked = true;
            other.checked = true;
            return true;
        } else {
            other.checkReason = checkReason;
        }
        return false;
    }
}
